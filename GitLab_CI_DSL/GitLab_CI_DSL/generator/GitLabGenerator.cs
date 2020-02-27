using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitLab_CI_DSL.metamodel.@default;
using GitLab_CI_DSL.metamodel.jobs;
using GitLab_CI_DSL.metamodel.pipeline;
using GitLab_CI_DSL.metamodel.stage;

namespace GitLab_CI_DSL.Generator
{
    public class GitLabYmlGenerator
    {
        public void CreateGitlabCiConfig(string fileName, string filePath, Pipeline pipeline)
        {
            var sb = new StringBuilder(); 
            
            sb.Append(parseDefaultJob(pipeline.Default));
            sb.Append(ParseStages(pipeline.Stages));
            pipeline.Stages.ForEach(stage => sb.Append(ParseJobs(stage)));

            if (!filePath.EndsWith("/"))
                filePath += "/";
            
            var path = filePath + fileName;

            if (System.IO.Directory.Exists(filePath))
            {
                Console.WriteLine($"Writing pipeline configuration to: {path}");
                System.IO.File.WriteAllText(@path, sb.ToString());
            }
            else
            {
                Console.WriteLine($"Directory: '{filePath}' does not exist!\nOutput will be printed in the console!\n");
                Console.WriteLine(sb.ToString());
            }
        }

        private string ParseStages(List<Stage> stages)
        {
            var stageNames = stages.Select(stage => stage.Name).ToList();
            var s = $"stages:\n";
            stageNames.ForEach(stage => s+= $"  - {stage}\n");

            return s;
        }


        private string parseDefaultJob(Default job)
        {
            if (job == null)
                return "";
            

            var sb = new StringBuilder();
            sb.AppendLine("default:");
            
            
            if (job.Image != null)
                sb.AppendLine($"  image: {job.Image}");


            var variables = job.EnvironmentVariables;
            if (variables != null)
            {
                sb.AppendLine($"  variables:");
                foreach (var (key, value) in variables)
                {
                    sb.AppendLine($"    {key}: \"{value}\"");
                }
            }

            return sb.ToString();
        }

        private string ParseJobs(Stage stage)
        {
            var sb = new StringBuilder();
            stage.Jobs.FindAll(job => job.Type == JobType.JOB ).ForEach( job => sb.Append(ParseJob(stage.Name, job)));

            return sb.ToString();
        }

        private string ParseJob(string stageName, Job job)
        {
            List<Job> extends = new List<Job>();
            GetJobParents(extends, job);

            string image = null; 
            var variables = new Dictionary<string, string>();
            var scripts = new List<string>();
            
            extends.ForEach(j =>
            {
                if (j.Image != null)
                    image = j.Image;

                if (j.Variables != null)
                {
                    foreach (var (key, value) in j.Variables)
                    {
                        variables[key] = value;
                    }
                }
                
                if(j.Scripts != null)
                    j.Scripts.ForEach(script => scripts.Add(script));
            });
            
            var sb = new StringBuilder();
            
            sb.AppendLine($"{job.Name}:");
            sb.AppendLine($"  stage: {stageName}");
            
            if (image != null)
                sb.AppendLine($"  image: {image}");
            

            if (variables.Count != 0)
            {
                sb.AppendLine("  variables:");
                foreach (var kv in variables)
                {
                    sb.AppendLine($"    {kv.Key}: \"{kv.Value}\"");
                }
            }
                
            if (scripts.Count != 0)
            {
                sb.AppendLine("  script:");
                scripts.ForEach(script => sb.AppendLine($"    - {script}"));
            }

            return sb.ToString();
        }
        
        
        private void GetJobParents(List<Job> jobs, Job job)
        {
            job.Extends?.ForEach( j => GetJobParents(jobs, j));
            jobs.Add(job);
        }
        
    }
    
    
    
}