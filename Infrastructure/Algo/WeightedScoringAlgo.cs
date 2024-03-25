using Application.Common.Algo;
using Application.Common.Services;
using Domain.Bids;

namespace Infrastructure.Algo;
public class WeightedScoringAlgo(IUnitOfWork uow) : IWeightedScoringAlgo
{
    double amountScore = -2.0;
    double providerAgeScore = 3;
    double providerRatingScore = 5;
    public async Task<Bid> GetRecommendedBid(IReadOnlyList<Bid> bids)
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

