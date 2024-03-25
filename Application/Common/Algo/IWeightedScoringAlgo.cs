using Domain.Bids;

namespace Application.Common.Algo;
public interface IWeightedScoringAlgo
{
    Task<Bid> GetRecommendedBid(IReadOnlyList<Bid> bids);
}