using System.Reflection;
using Rougamo;
using Rougamo.Context;

namespace Common.Check;

[AttributeUsage(AttributeTargets.Method)]
public class EnableParameterCheckAttribute : MoAttribute
{
	public override void OnEntry(MethodContext context)
	{
		var parameters = context.Method.GetParameters();
		foreach (var (parameter, index) in parameters.Select((value, index) => (value, index)))
		{
			var paramNotNullAttr = parameter.GetCustomAttribute<NotNullAttribute>();
			var attributes = new List<IParameterCheck?> { paramNotNullAttr };

			foreach (var attribute in attributes)
			{
				attribute?.Check(context.Arguments[index]);
			}
		}

		base.OnEntry(context);
	}
}