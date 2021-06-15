using Sharkingbird.Jarvis.Core.Models.ValueTypes.Exceptions;
using System;

namespace Sharkingbird.Jarvis.Core.Models.ValueTypes
{
  public struct BudgetNameValueType
  {
    private readonly string _nameParameter;

    private BudgetNameValueType(string nameParameterParam)
    {
      _nameParameter = nameParameterParam;
    }

    public override string ToString()
    {
      return _nameParameter;
    }

    public static implicit operator BudgetNameValueType(string nameParam)
    {
      return From(nameParam);
    }

    public static implicit operator string(BudgetNameValueType valueParam)
    {
      return valueParam._nameParameter;
    }

    public static bool operator ==(BudgetNameValueType v1Param, BudgetNameValueType v2Param)
    {
      return v1Param.equals(v2Param);
    }

    public static bool operator !=(BudgetNameValueType v1Param, BudgetNameValueType v2Param)
    {
      return !v1Param.equals(v2Param);
    }

    public override bool Equals(object objParam)
    {
      switch (objParam)
      {
        case BudgetNameValueType other:
          return equals(other);
        default:
          return false;
      }
    }

    private bool equals(BudgetNameValueType otherParam)
    {
      return string.Equals(_nameParameter, otherParam._nameParameter, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
      return _nameParameter.ToLower().GetHashCode();
    }

    public static BudgetNameValueType From(string nameParameterParam)
    {
      if (nameParameterParam == null)
      {
        throw new InvalidBudgetNameException("Name was null.");
      }

      if (string.IsNullOrWhiteSpace(nameParameterParam))
      {
        throw new InvalidBudgetNameException("Name was empty.");
      }

      return new BudgetNameValueType(nameParameterParam.Trim());
    }
  }
}
