using System;

namespace Pipelines
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("######## Dot Net Core ########");
            new GitLabDslPipeline();
            
            Console.WriteLine("######## Simple Maven ########");
            new SimpleMavenPipeline();

            Console.WriteLine("\n######## Complex Maven ########");
            new MavenProjPipeline();
        }
    }
} 