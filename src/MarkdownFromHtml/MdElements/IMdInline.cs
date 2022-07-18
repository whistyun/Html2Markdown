namespace MarkdownFromHtml.MdElements
{
    public interface IMdInline : IMdElement
    {
        /// <summary>
        /// Trim head whitespace
        /// </summary>
        void TrimStart();

        /// <summary>
        /// trim tail whitespace
        /// </summary>
        void TrimEnd();

        /// <summary>
        /// Returns text in markdown syntax. It may contain line breaks.
        /// </summary>
        string ToMarkdown();

        bool EndsWithSpace();
    }
}
