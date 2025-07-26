You are a skilled software developer tasked with analyzing the core modules of a project. Your goal is to provide a high-level overview of these modules based on the project's onboarding documentation and git history.

First, carefully read the project onboarding document:

<project_onboarding_doc>
#file:onboarding.md
</project_onboarding_doc>

Based on this document, identify the core modules of the project. For each core module, you will conduct an analysis using git history and various tools. Perform your initial analysis inside <module_analysis> tags within your thinking block (step 1), then when you encounter <perform_action> block, exit thinking block and use tool calling to perform git commands for further analysis via exploring the project (step 2).

<module_analysis>
1. List all core modules identified from the onboarding document:
   [List modules here]

</module_analysis>

Then use git commands to perform next steps:
<perform_action>
2. For each core module:
   a. Extract and quote relevant information about the module from the onboarding document.

   b. Git History Analysis:
      Command: git --no-pager log --pretty=format:"Commit: %H%nAuthor: %an <%ae>%nDate: %ad%nSubject: %s%n%n------------------------------------------------------------" --date=iso -n 10  -- [path-to-module]
      [Record the output here]
      [Identify and note patterns or themes in the commit messages]

   c. Module Analysis Summary:
      Module Name: [Name of the module]
      Role: [Describe the primary purpose and responsibilities in 2-3 sentences]
      Structure: [Outline the key components and organization in 2-3 sentences]
      Recent Focus: [Identify areas of recent development activity in 2-3 sentences]

[Repeat steps a-c for each core module]
</perform_action>

3. Cross-module analysis:
   [Compare and contrast modules, noting similarities and differences]
   [Summarize findings across all modules, noting any patterns or relationships]

After completing your analysis, compile a high-level overview that summarizes your findings. Your final output should be structured as follows:

Core Modules Overview:

1. [Module Name 1]
   - Role: [Description in 2-3 sentences based on onboarding document and performed analysis]
   - Structure: [Key components]
   - Recent Focus: [Areas of recent activity]

2. [Module Name 2]
   - Role: [Description in 2-3 sentences based on onboarding document and performed analysis]
   - Structure: [Key components]
   - Recent Focus: [Areas of recent activity]

[Continue for all identified core modules]

Ensure that your overview is based on the detailed analysis you conducted, including insights from the git history and tool usage. Focus on the most important aspects of each module, providing a concise yet informative summary. Your final output should be saved in .ai/onboarding/core-modules.md file and consist only of the Core Modules Overview and should not duplicate or rehash any of the work you did in the thinking block.