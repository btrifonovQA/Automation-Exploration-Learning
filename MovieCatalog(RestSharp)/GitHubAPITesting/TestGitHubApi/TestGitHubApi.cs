using NUnit.Framework.Internal;
using RestSharpServices;
using RestSharpServices.Models;

namespace TestGitHubApi
{
    [TestFixture]
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private readonly string token = "GitHub Token goes here";  // place valid token when running
        private readonly string repo = "GitHub-Api-Test";   //place valid repo when running
        private static int randomNum;

        private static long lastCreatedIssueNumber;
        private static long lastCreatedCommentId;
        private static string lastCreatedCommentBody;

        [SetUp]
        public void Setup()
        {
            client = new GitHubApiClient("baseURL", "username", token); //replace url and username with valid data when running

            Random random = new Random();
            randomNum = random.Next(10000);
        }


        [Test, Order(1)]
        public void Test_GetAllIssuesFromARepo()
        {
            List<Issue>? issues = client.GetAllIssues(repo);

            Assert.That(issues, Has.Count.GreaterThan(1), "There should be more than one issue");

            foreach (Issue issue in issues)
            {
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(issue.Id, Is.Positive);
                    Assert.That(issue.Number, Is.Positive);
                    Assert.That(issue.Title, Is.Not.Empty);
                }
            }
        }

        [Test, Order(2)]
        public void Test_GetIssueByValidNumber()
        {
            int issueNumber = 1;

            Issue? issue = client.GetIssueByNumber(repo, issueNumber);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(issue, Is.Not.Null);
                Assert.That(issue.Id, Is.Positive);
                Assert.That(issue.Number, Is.EqualTo(issueNumber));
            }
        }

        [Test, Order(3)]
        public void Test_GetAllLabelsForIssue()
        {
            int issueNumber = 1;

            List<Label>? labels = client.GetAllLabelsForIssue(repo, issueNumber);

            Assert.That(labels, Is.Not.Empty);

            foreach (Label label in labels)
            {
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(label.Id, Is.Positive);
                    Assert.That(label.Name, Is.Not.Empty);
                }

                //Print label
                Console.WriteLine("Label: " + label.Id + " - Name: " + label.Name);
            }
        }

        [Test, Order(4)]
        public void Test_GetAllCommentsForIssue()
        {
            int issueNumber = 1;

            List<Comment>? comments = client.GetAllCommentsForIssue(repo, issueNumber);

            Assert.That(comments, Is.Not.Empty);

            foreach (Comment comment in comments)
            {
                using (Assert.EnterMultipleScope())
                {
                    Assert.That(comment.Id, Is.Positive);
                    Assert.That(comment.Body, Is.Not.Empty);
                }

                //Print label
                Console.WriteLine("Comment: " + comment.Id + " - Body: " + comment.Body);
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            string title = $"Method.Post Title Test_{randomNum}";
            string body = $"Method.Post Body Test_{randomNum}";

            Issue? createdIssue = client.CreateIssue(repo, title, body);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(createdIssue.Id, Is.Positive);
                Assert.That(createdIssue.Number, Is.Positive);
                Assert.That(createdIssue.Title, Is.Not.Empty);
                Assert.That(createdIssue.Title, Is.EqualTo(title));
            }

            Console.WriteLine(createdIssue.Number);
            lastCreatedIssueNumber = createdIssue.Number;
        }

        [Test, Order(6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            string body = $"Random comment body_{randomNum}";
            Comment? createdComment = client.CreateCommentOnGitHubIssue(repo, lastCreatedIssueNumber, body);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(createdComment.Id, Is.Positive);
                Assert.That(createdComment.Body, Is.EqualTo(body));
            }

            Console.WriteLine(createdComment.Id);
            lastCreatedCommentId = createdComment.Id;
            lastCreatedCommentBody = body;
        }

        [Test, Order(7)]
        public void Test_GetCommentById()
        {
            Comment? comment = client.GetCommentById(repo, lastCreatedCommentId);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(comment, Is.Not.Null);
                Assert.That(comment.Id, Is.EqualTo(lastCreatedCommentId));
                Assert.That(comment.Body, Is.EqualTo(lastCreatedCommentBody));
            }
        }


        [Test, Order(8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            string editCommentBody = "Newly edited comment body";

            Comment? editedComment = client.EditCommentOnGitHubIssue(repo, lastCreatedCommentId, editCommentBody);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(editedComment, Is.Not.Null);
                Assert.That(editedComment.Id, Is.EqualTo(lastCreatedCommentId));
                Assert.That(editedComment.Body, Is.EqualTo(editCommentBody));
            }
        }

        [Test, Order(9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            bool deleteCommandResult = client.DeleteCommentOnGitHubIssue(repo, lastCreatedCommentId);

            Assert.That(deleteCommandResult, Is.True);
        }
    }
}

