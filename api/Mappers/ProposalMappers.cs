using api.Dtos.Proposal;
using api.Models;

namespace api.Mappers.ProposalMappers;

public static class ProposalMapper
{
    public static ProposalDto ToProposalDto(this Proposal proposal)
    {
        return new ProposalDto
        {
            Id = proposal.Id,
            UserId = proposal.UserId,
            UserName = $"{proposal.User.Name} {proposal.User.Surname}",
            CreatedAt = proposal.CreatedAt,
            Title = proposal.Title,
            Topic = proposal.Topic,
            Purpose = proposal.Purpose,
            Explanation = proposal.Explanation
        };
    }

    public static Proposal ToProposal(this CreateProposalRequestDto proposalDto)
    {
        return new Proposal
        {
            UserId = proposalDto.UserId,
            CreatedAt = proposalDto.CreatedAt == default ? DateTime.UtcNow : proposalDto.CreatedAt,
            Title = proposalDto.Title,
            Topic = proposalDto.Topic,
            Purpose = proposalDto.Purpose,
            Explanation = proposalDto.Explanation
        };
    }

    public static Proposal ToProposal(this UpdateProposalRequestDto proposalDto)
    {
        return new Proposal
        {
            Title = proposalDto.Title,
            Topic = proposalDto.Topic,
            Purpose = proposalDto.Purpose,
            Explanation = proposalDto.Explanation
        };
    }
}