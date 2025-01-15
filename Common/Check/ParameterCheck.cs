using System.Reflection;
using Rougamo;
using Rougamo.Context;

namespace Common.Check;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public class ParameterCheck : MoAttribute
{
	public override void OnEntry(MethodContext context)
	{
		var parameters = context.Method.GetParameters();
		foreach (var (parameter, index) in parameters.Select((value, index) => (value, index)))
		{
			var parameterAttributes = parameter.GetCustomAttributes();

			foreach (var attribute in parameterAttributes)
			{
				if (attribute is IParameterCheck parameterCheck)
				{
					parameterCheck.Check(context.Arguments[index]);
				}
			}
		}

		base.OnEntry(context);
	}
}