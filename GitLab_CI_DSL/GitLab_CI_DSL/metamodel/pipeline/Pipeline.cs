using System.Collections.Generic;

namespace GitLab_CI_DSL
{
    public class Pipeline
    {
        private string Name { get; }

        public Default Default { get; set; }

        public List<Stage> Stages { get; }

        public List<Job> Extensions { get; }


        public Pipeline(string name)
        {
            this.Name = name;
            Extensions = new List<Job>();
            Stages = new List<Stage>();
        }


        public void AddStage(Stage stage)
        {
            this.Stages.Add(stage);
        }

        public void AddExtension(Job extension)
        {
            this.Extensions.Add(extension);
        }
        

    }
}