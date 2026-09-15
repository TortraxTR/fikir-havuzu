namespace api.Dtos.ProposalFile
{
    // What a download response needs; separate from ProposalFileDto so file listings
    // don't carry the file content. Content is streamed from R2 rather than buffered.
    public record ProposalFileContent(string FileName, string ContentType, Stream Content);
}
