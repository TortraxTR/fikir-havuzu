namespace api.Dtos.ProposalFile
{
    // What a download response needs; separate from ProposalFileDto so file listings
    // don't carry the raw bytes.
    public record ProposalFileContent(string FileName, string ContentType, byte[] Content);
}
