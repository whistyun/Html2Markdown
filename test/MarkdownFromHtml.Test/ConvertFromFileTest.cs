using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using VerifyNUnit;

namespace MarkdownFromHtml.Test {
	[TestFixture]
	public class ConvertFromFileTest {
		private string _testPath;

		[SetUp]
		public void SetUp() {
			_testPath = TestPath();
		}

		[Test]
		public Task ConvertFile_WhenReadingInHtmlFile_ThenConvertToMarkdown()
		{
			var sourcePath = Path.Combine(_testPath ,"TestHtml.txt");

			return CheckFileConversion(sourcePath);
		}

		private static string TestPath()
		{
			var asm = Assembly.GetCallingAssembly();
			var asmDir = Path.GetDirectoryName(asm.Location);
			var route = Path.Combine(asmDir, @"..\..\..\Files");
			var environmentPath = System.Environment.GetEnvironmentVariable("Test.Path");

			return environmentPath ?? route;
		}

		private static Task CheckFileConversion(string path)
		{
			var converter = new Converter();

			var result = converter.ConvertFile(path);

			return Verifier.Verify(result);
		}
	}
}