namespace GodotUtils.InstanceResolver.Internal;

public interface IHasParams<TParams>
{
    TParams Map(TParams parameters);
}
