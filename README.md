# Model-Driven Software Development
Repository for MDSD course on SDU.

## Future ideas for internal pipeline DSL
- Artifacts from pipeline
    ```
    Stage("save_artifacts").
        Job("javadoc").
            Artifact()
    
    ```

- Rules for which branches the job apply
    ```
    Stage("deployment").
        Job("deploy").
            Only().
                Branch("master")
    ```
