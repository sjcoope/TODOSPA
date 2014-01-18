using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Test.Common.Helper
{
    public static class FileHelper
    {
        public static string GetTextFromFile(string fileName)
        {
            StreamReader reader = null;
            try
            {
                // Get the path to the folder holding the messages
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Read the file.
                reader = File.OpenText(Path.Combine(basePath, fileName));
                return reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
        }
    }
}
