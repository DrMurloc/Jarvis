using Sharkingbird.Jarvis.Core.Models.ValueTypes.Exceptions;
using System;

namespace Sharkingbird.Jarvis.Core.Models.ValueTypes
{
  public struct RecurringTransactionNameValueType
  {
    private readonly string _nameParameter;

    private RecurringTransactionNameValueType(string nameParameterParam)
    {
      _nameParameter = nameParameterParam;
    }

    public override string ToString()
    {
      return _nameParameter;
    }

    public static implicit operator RecurringTransactionNameValueType(string nameParam)
    {
      return From(nameParam);
    }

    public static implicit operator string(RecurringTransactionNameValueType valueParam)
    {
      return valueParam._nameParameter;
    }

    public static bool operator ==(RecurringTransactionNameValueType v1Param, RecurringTransactionNameValueType v2Param)
    {
      return v1Param.equals(v2Param);
    }

    public static bool operator !=(RecurringTransactionNameValueType v1Param, RecurringTransactionNameValueType v2Param)
    {
      return !v1Param.equals(v2Param);
    }

    public override bool Equals(object objParam)
    {
      switch (objParam)
      {
        case RecurringTransactionNameValueType other:
          return equals(other);
        default:
          return false;
      }
    }

    private bool equals(RecurringTransactionNameValueType otherParam)
    {
      return string.Equals(_nameParameter, otherParam._nameParameter, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
      return _nameParameter.ToLower().GetHashCode();
    }

    public static RecurringTransactionNameValueType From(string nameParameterParam)
    {
      if (nameParameterParam == null)
      {
        throw new InvalidRecurringTransactionName("Name was null.");
      }

      if (string.IsNullOrWhiteSpace(nameParameterParam))
      {
        throw new InvalidRecurringTransactionName("Name was empty.");
      }

      return new RecurringTransactionNameValueType(nameParameterParam.Trim());
    }
  }
}
