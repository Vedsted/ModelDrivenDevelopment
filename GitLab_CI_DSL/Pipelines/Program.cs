using System;

namespace Pipelines
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("######## DotNet Core ########");
            new DotNetPipeline();

            Console.WriteLine("######## Maven 1 ########");
            new MavenPipeline();

            Console.WriteLine("\n######## Maven 2 ########");
            new MavenPipeline2();

            Console.WriteLine("\n######## Gradle ########");
            new GradlePipeline();
            
            Console.WriteLine("\n######## Inheritance Example ########");
            new InheritancePipeline();
            
        }
    }
} 