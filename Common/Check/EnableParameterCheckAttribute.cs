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
			var paramNotNullAttr = parameter.GetCustomAttribute<ParamNotNullAttribute>();
			var paramPositiveAttr = parameter.GetCustomAttribute<ParamPositiveAttribute>();
			var paramNegativeAttr = parameter.GetCustomAttribute<ParamNegativeAttribute>();
			var attributes = new List<IParameterCheck?> { paramNotNullAttr, paramPositiveAttr, paramNegativeAttr };

			foreach (var attribute in attributes)
			{
				attribute?.Check(context.Arguments[index]);
			}
		}

		base.OnEntry(context);
	}
}