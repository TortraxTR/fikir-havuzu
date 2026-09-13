using api.Dtos.ProposalFile;
using api.Models;

namespace api.Mappers.ProposalFileMappers
{
    public static class ProposalFileMapper
    {
        public static ProposalFileDto ToProposalFileDto(this ProposalFile proposalFile)
        {
            return new ProposalFileDto
            {
                Id = proposalFile.Id,
                ProposalId = proposalFile.ProposalId,
                FileName = proposalFile.FileName,
                ContentType = proposalFile.ContentType,
                SizeBytes = proposalFile.Content.LongLength
            };
        }
    }
}
