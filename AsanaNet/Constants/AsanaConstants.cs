namespace AsanaNet.Constants;

/// <summary>
/// Contains constant values used in the Asana API client.
/// </summary>
public static class AsanaConstants
{
    /// <summary>
    /// The default base URL for the Asana API.
    /// </summary>
    public const string DefaultBaseUrl = "https://app.asana.com/api/1.0/";

    /// <summary>
    /// API endpoint paths
    /// </summary>
    public static class Endpoints
    {
        public const string Tasks = "tasks";
        public const string Workspaces = "workspaces";
        public const string Teams = "teams";
        public const string Projects = "projects";
        public const string Users = "users";
        public const string Stories = "stories";
        public const string Attachments = "attachments";
        public const string Sections = "sections";
        public const string Organizations = "organizations";
        public const string Tags = "tags";
        public const string Events = "events";
        public const string Dependencies = "dependencies";
        public const string Subtasks = "subtasks";
        public const string CustomFields = "custom_fields";
        public const string Followers = "followers";
        public const string AddFollowers = "addFollowers";
        public const string RemoveFollowers = "removeFollowers";
        public const string AddLike = "addLike";
        public const string RemoveLike = "removeLike";
        public const string AddTask = "addTask";
        public const string Duplicate = "duplicate";
        public const string Search = "search";
        public const string Me = "me";
        public const string Comments = "comments";
        public const string Hearts = "hearts";
    }

    /// <summary>
    /// Common API parameters
    /// </summary>
    public static class Parameters
    {
        public const string Data = "data";
        public const string File = "file";
        public const string Url = "url";
        public const string Text = "text";
        public const string Name = "name";
        public const string Notes = "notes";
        public const string Completed = "completed";
        public const string DueOn = "due_on";
        public const string DueAt = "due_at";
        public const string StartOn = "start_on";
        public const string Assignee = "assignee";
        public const string Projects = "projects";
        public const string Workspace = "workspace";
        public const string Parent = "parent";
        public const string Dependencies = "dependencies";
        public const string CustomFields = "custom_fields";
        public const string InsertBefore = "insert_before";
        public const string InsertAfter = "insert_after";
        public const string IncludeSubtasks = "include_subtasks";
        public const string IncludeDependencies = "include_dependencies";
        public const string Followers = "followers";
        public const string Tags = "tags";
        public const string Color = "color";
        public const string Enabled = "enabled";
        public const string ResourceType = "resource_type";
        public const string Sync = "sync";
        public const string OptFields = "opt_fields";
        public const string Limit = "limit";
        public const string Offset = "offset";
        public const string CompletedSince = "completed_since";
        public const string ModifiedSince = "modified_since";
    }

    /// <summary>
    /// Default values and limits
    /// </summary>
    public static class Defaults
    {
        public const int DefaultPageSize = 100;
        public const int MaxRetries = 3;
        public const int RetryDelayMilliseconds = 1000;
        public const int MaxBatchSize = 100;
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        public const string DateFormat = "yyyy-MM-dd";
    }

    /// <summary>
    /// Resource types
    /// </summary>
    public static class ResourceTypes
    {
        public const string Task = "task";
        public const string Project = "project";
        public const string Section = "section";
        public const string Tag = "tag";
        public const string Story = "story";
        public const string CustomField = "custom_field";
        public const string User = "user";
        public const string Team = "team";
        public const string Workspace = "workspace";
        public const string Attachment = "attachment";
    }
} 