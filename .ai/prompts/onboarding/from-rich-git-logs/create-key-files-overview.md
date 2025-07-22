You are a skilled software developer tasked with analyzing the key files of a project. Your goal is to provide a high-level overview based on the project's top files list, git history, and file system analysis.

First, carefully examine the list of top files and their change history:
<top_files_list>
#file:top-modified-files.txt 
</top_files_list>

Please complete this analysis in THREE DISTINCT PHASES. EACH PHASE MUST BE COMPLETED BEFORE MOVING TO THE NEXT:

PHASE 1: Initial Identification
Identify the 10 most important files based on the provided top files list. List them by full path and briefly note why each appears significant based on change frequency and contributor involvement.

PHASE 2: Git History Analysis
For each of the 10 key files identified:
1. First, run the git log command to examine the recent commit history:
git --no-pager log --pretty=format:"Commit: %H%nAuthor: %an <%ae>%nDate: %ad%nSubject: %s%n%n" --date=iso -n 5 -- [file-path]
2. Analyze the commit messages to identify patterns and recent focus areas
3. Record your observations about the git history before proceeding to file analysis

YOU MUST COMPLETE PHASE 2 BEFORE MOVING TO PHASE 3.

PHASE 3: File Content Analysis
After completing the git history analysis for all 10 files, examine the actual content of each file:
1. Read the contents of each file to understand its purpose, organization, and key components.
2. For each file, provide:
- File purpose and function (2-3 sentences)
- Content organization and key components (2-3 sentences)
- How the file connects to other parts of the system (1-2 sentences)

Final Deliverable:
After completing all three phases, summarize your findings in a "Key Files Overview" section that includes:
1. The role of each file in the system
2. Recent development focus areas based on git history
3. Key architectural insights about how these files work together

Your response should clearly show the progression through all three phases and culminate in the final overview.