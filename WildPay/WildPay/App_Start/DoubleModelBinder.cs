﻿using System;
using System.Globalization;
using System.Web.Mvc;

public class DoubleModelBinder : DefaultModelBinder
{
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        object result = null;

        // Don't do this here!
        // It might do bindingContext.ModelState.AddModelError
        // and there is no RemoveModelError!
        // 
        // result = base.BindModel(controllerContext, bindingContext);

        string modelName = bindingContext.ModelName;
        string attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;

        // in decimal? binding attemptedValue can be Null
        if (attemptedValue != null)
        {
            // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
            // Both "." and "," should be accepted, but aren't.
            string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            string alternateSeperator = (wantedSeperator == "," ? "." : ",");

            if (attemptedValue.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1
                && attemptedValue.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
            {
                attemptedValue = attemptedValue.Replace(alternateSeperator, wantedSeperator);
            }

            try
            {
                if (bindingContext.ModelMetadata.IsNullableValueType && string.IsNullOrWhiteSpace(attemptedValue))
                {
                    return null;
                }

                result = double.Parse(attemptedValue, NumberStyles.Any);
            }
            catch (FormatException e)
            {
                bindingContext.ModelState.AddModelError(modelName, e);
            }
        }

        return result;
    }
}