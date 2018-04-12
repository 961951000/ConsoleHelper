﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace ConsoleHelper.Helpers
{
    public class GitHubManager
    {
        public readonly IEnumerable<string> _gitHubTokens;

        public readonly IGitHubClient _githubClient;

        public GitHubManager()
        {
            _gitHubTokens = ConfigurationManager.AppSettings["GitHubTokens"].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            _githubClient = new GitHubClient(new ProductHeaderValue("ApplicationName"))
            {
                Credentials = new Credentials(_gitHubTokens.FirstOrDefault())
            };
        }

        public void InitializeGithubClient()
        {
            var gitHubToken = _githubClient.Connection.Credentials.GetToken();
            var tempTokensList = string.IsNullOrWhiteSpace(gitHubToken) ? _gitHubTokens : _gitHubTokens.Except(new List<string> { gitHubToken });
            _githubClient.Connection.Credentials = new Credentials(PickRandom(tempTokensList.ToList()));
        }

        public async Task<IEnumerable<KeyValuePair<string, User>>> GetUserListAsync(string gitHubTokens)
        {
            var tokens = gitHubTokens.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var tasks = tokens.Select(GetCurrentUserAsync);

            return await Task.WhenAll(tasks);
        }

        public async Task<string> GetPullRequestBranchAsync(string owner, string name, string sha)
        {
            var pullRequest = await GetPullRequest(owner, name, sha).ConfigureAwait(false);

            return pullRequest?.Base.Ref;
        }

        public async Task<int> GetPullRequestNumberFromShaAsync(string owner, string name, string sha)
        {
            var requiredPullRequest = await GetPullRequest(owner, name, sha).ConfigureAwait(false);

            return requiredPullRequest.Number;
        }

        public async Task<IEnumerable<PullRequestFile>> GetPullRequestPropertiesAsync(string owner, string name, int pullRequestNumber)
        {
            var pullRequestFiles = await _githubClient.PullRequest.Files(owner, name, pullRequestNumber);

            return pullRequestFiles;
        }

        public async Task<IEnumerable<PullRequest>> GetAllOpenPullRequestsAsync(string owner, string name)
        {
            return await _githubClient.PullRequest.GetAllForRepository(owner, name);
        }

        private async Task<string> GetFileContent(string owner, string name, PullRequestFile file)
        {
            var blob = await _githubClient.Git.Blob.Get(owner, name, file.Sha);
            var fileData = Convert.FromBase64String(blob.Content);

            return Encoding.UTF8.GetString(fileData);
        }

        private async Task<PullRequest> GetPullRequest(string owner, string name, string sha)
        {
            var pullRequests = await GetAllOpenPullRequestsAsync(owner, name);
            var requiredPullRequest = pullRequests.FirstOrDefault(x => x.Head?.Sha != null && x.Head.Sha.Equals(sha));

            return requiredPullRequest;
        }

        private async Task<KeyValuePair<string, User>> GetCurrentUserAsync(string token)
        {
            _githubClient.Connection.Credentials = new Credentials(token);

            return new KeyValuePair<string, User>(token, await _githubClient.User.Current());
        }

        private string PickRandom(IList<string> gitHubTokens)
        {
            var random = new Random();
            var randomTokenPosition = random.Next(0, gitHubTokens.Count);

            return gitHubTokens[randomTokenPosition].Trim();
        }

        public async Task<IEnumerable<Label>> GetLabelsFromRequestNumberAsync(string owner, string name, int pullRequestNumber, params string[] labelsToCheck)
        {
            var issue = await _githubClient.Issue.Get(owner, name, pullRequestNumber);

            return issue.Labels;
        }

        public async Task<Reference> CreateBranchAsync(string owner, string name, string branch, string baseRef)
        {
            var master = await _githubClient.Repository.Branch.Get(owner, name, baseRef);

            return await _githubClient.Git.Reference.Create(owner, name, new NewReference($"refs/heads/{branch}", master.Commit.Sha));
        }

        public async Task DeleteBranchAsync(string owner, string name, string branch)
        {
            await _githubClient.Git.Reference.Delete(owner, name, $"heads/{branch}");
        }

        public async Task<PullRequest> CreatePullRequestAsync(string owner, string name, string title, string branch, string baseRef)
        {
            return await _githubClient.PullRequest.Create(owner, name, new NewPullRequest(title, branch, baseRef));
        }

        public async Task OpenPullRequestAsync(string owner, string name, int number)
        {
            await _githubClient.PullRequest.Update(owner, name, number, new PullRequestUpdate { State = ItemState.Open });
        }

        public async Task ClosePullRequestAsync(string owner, string name, int number)
        {
            await _githubClient.PullRequest.Update(owner, name, number, new PullRequestUpdate { State = ItemState.Closed });
        }

        public async Task<RepositoryContentChangeSet> CreateFileAsync(string owner, string name, string path, string message, string content, string branch)
        {
            return await _githubClient.Repository.Content.CreateFile(owner, name, path, new CreateFileRequest(message, content, branch));
        }

        public async Task DeleteFileAsync(string owner, string name, string path, string message, string content, string branch)
        {
            await _githubClient.Repository.Content.DeleteFile(owner, name, path, new DeleteFileRequest(message, content, branch));
        }

        public async Task<RepositoryContentChangeSet> UpdateFileAsync(string owner, string name, string path, string message, string content, string branch)
        {
            var existingFile = await _githubClient.Repository.Content.GetAllContentsByRef(owner, name, path, branch);

            return await _githubClient.Repository.Content.UpdateFile(owner, name, path, new UpdateFileRequest(message, content, existingFile.First().Sha, branch));
        }

        public async Task Merge(string owner, string name, int number, string commitTitle, string commitMessage)
        {
            await _githubClient.PullRequest.Merge(owner, name, number, new MergePullRequest
            {
                CommitTitle = commitTitle,
                CommitMessage = commitMessage
            });
        }
    }
}
