Properties = "Variables"
Items = "Object Lists" (Identity (~Key) + Metadata)
Targets composed of Tasks = (Build) Logic

Projects: .*proj
Options & Defaults (before the main project): .props
Logic (after the main project): .targets
Names prefixed with underscore (_): "private"

Evaluation order:
1. Evaluate environment variables
2. Evaluate imports and properties
3. Evaluate item definitions
4. Evaluate items
5. Evaluate UsingTask elements
6. Evaluate targets