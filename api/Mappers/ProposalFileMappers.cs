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
                File = proposalFile.File
            };
        }

        public static ProposalFile ToProposalFile(this CreateProposalFileRequestDto createDto)
        {
            return new ProposalFile
            {
                ProposalId = createDto.ProposalId,
                File = createDto.File
            };
        }

        public static ProposalFile ToProposalFile(this UpdateProposalFileRequestDto updateDto)
        {
            return new ProposalFile
            {
                ProposalId = updateDto.ProposalId,
                File = updateDto.File
            };
        }
    }
}
