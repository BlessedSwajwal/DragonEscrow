using Application.Common.Services;
using Domain.Bids;

namespace Infrastructure.Algo;
public class WeightedScoringAlgo(IUnitOfWork uow)
{
    double amountScore = 0.8;
    double providerAgeScore = 1.2;
    double providerRatingScore = 1.4;
    public async Task<Bid> GetRecommendedBid(List<Bid> bids)
    {
        if (!bids.Any()) return Bid.Empty;
        var providerIds = bids.Select(b => b.BidderId).ToList();
        //Get list of provider
        var providerList = await uow.ProviderRepository.GetAllByIdAsync(providerIds);

        var bid = bids.Select(b =>
        {
            var providerForTheBid = providerList.Single(p => b.BidderId == p.Id);
            var providerAge = (DateTime.Now - providerForTheBid.CreatedDate).Days;
            double score = (b.ProposedAmount * amountScore) + (providerAge * providerAgeScore) + (providerForTheBid.AvgRating * providerRatingScore);

            return new BidWithScore(b, score);
        }).MaxBy(bws => bws.Score)!.Bid;

        return bid;
    }
}

internal record BidWithScore(Bid Bid, double Score);

