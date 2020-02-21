using System;

namespace MavenApp_CI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("######## Simple Maven ########");
            new SimpleMavenPipeline();

            Console.WriteLine("\n######## Complex Maven ########");
            new MavenProjPipeline();
        }
    }
}