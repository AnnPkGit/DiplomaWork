namespace Application.MediaItems.Commands.Common;

public abstract class CreateBaseMediaItemModel
{
    protected CreateBaseMediaItemModel() {}
    protected CreateBaseMediaItemModel(Stream file, string name, string type)
    {
        File = file;
        Name = name;
        Type = type;
    }

    public Stream File { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
}