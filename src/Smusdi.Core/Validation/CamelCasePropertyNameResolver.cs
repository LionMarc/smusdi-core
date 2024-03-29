﻿using System.Linq.Expressions;
using System.Reflection;
using FluentValidation.Internal;
using Smusdi.Core.Helpers;

namespace Smusdi.Core.Validation;

public static class CamelCasePropertyNameResolver
{
    public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
    {
        return DefaultPropertyNameResolver(memberInfo, expression).ToCamelCase();
    }

    private static string DefaultPropertyNameResolver(MemberInfo memberInfo, LambdaExpression expression)
    {
        if (expression != null)
        {
            var chain = PropertyChain.FromExpression(expression);
            if (chain.Count > 0)
            {
                return chain.ToString();
            }
        }

        if (memberInfo != null)
        {
            return memberInfo.Name;
        }

        return string.Empty;
    }
}
