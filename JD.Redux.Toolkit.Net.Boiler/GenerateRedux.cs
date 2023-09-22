using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;

namespace JD.Redux.Toolkit.Net.Boiler;

public class GenerateRedux { 

    public GenerateRedux() {
    }

    public void Build()
    {
        // Get the assembly containing the controllers
        Assembly controllersAssembly = Assembly.GetExecutingAssembly();

        // Find all controller types
        var controllerTypes = controllersAssembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

        // Create a folder for the generated Redux files
        Directory.CreateDirectory("Redux");
        Directory.CreateDirectory("Redux/controllers");

        // Iterate through each controller type
        foreach (var controllerType in controllerTypes)
        {
            // Get the controller name without the "Controller" suffix
            string controllerName = controllerType.Name.Replace("Controller", "");

            // Create a folder for the current controller and its models
            Directory.CreateDirectory($"Redux/controllers/{controllerName}");
            //Directory.CreateDirectory($"Redux/controllers/{controllerName}/models");

            // Find all public methods in the controller
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !m.IsSpecialName && m.DeclaringType == controllerType);

            // Generate the thunk and slice files for the current controller
            GenerateThunkAndSliceFiles(controllerName, methods);
        }

        // Generate the store.ts file
        GenerateStoreFile(controllerTypes);
    }


    static void GenerateThunkAndSliceFiles(string controllerName, IEnumerable<MethodInfo> methods)
    {
        string thunkFileName = $"Redux/controllers/{controllerName}/{controllerName}Thunks.ts";
        string sliceFileName = $"Redux/controllers/{controllerName}/{controllerName}Slices.ts";

        var thunkContentBuilder = new StringBuilder();
        var sliceContentBuilder = new StringBuilder();

        // Iterate through each method (endpoint) in the controller
        foreach (var method in methods)
        {
            string endpointName = method.Name;
            string apiRoute = $"/api/{controllerName}/{endpointName}";

            // Generate the thunk and slice content for the current endpoint
            thunkContentBuilder.AppendLine(GenerateThunkContent(controllerName, endpointName, apiRoute, method));
            sliceContentBuilder.AppendLine(GenerateSliceContent(controllerName, endpointName));

            // Generate the model files for the endpoint response and parameters
            //GenerateModelFiles(controllerName, endpointName, method);
        }

        // Write the thunk and slice content to the respective files
        File.WriteAllText(thunkFileName, thunkContentBuilder.ToString());
        File.WriteAllText(sliceFileName, sliceContentBuilder.ToString());

        Console.WriteLine($"Generated {thunkFileName} and {sliceFileName}.");
    }

    static string GenerateThunkContent(string controllerName, string endpointName, string apiRoute, MethodInfo method)
    {
        var parameters = method.GetParameters();
        string paramsType = parameters.Length > 0 ? $"{controllerName}{endpointName}Params" : "void";

        return $@"export const fetch{controllerName}{endpointName} = createAsyncThunk(
  '{controllerName.ToLower()}/fetch{controllerName}{endpointName}',
  async (params: {paramsType}): Promise<any> => {{
    const response = await fetch('{apiRoute}', {{
      method: 'GET',
      headers: {{
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      }},
      body: JSON.stringify(params),
    }});
    const data: {controllerName}{endpointName}Response = await response.json();
    return data;
  }}
);
";
    }

    static string GenerateSliceContent(string controllerName, string endpointName)
    {
        return $@"const {controllerName.ToLower()}{endpointName}Slice = createSlice({{
  name: '{controllerName.ToLower()}{endpointName}',
  initialState: {{ {controllerName.ToLower()}{endpointName}Data: any as {controllerName}{endpointName}Response, status: 'idle', error: null }},
  reducers: {{}},
  extraReducers: (builder) => {{
    builder
      .addCase(fetch{controllerName}{endpointName}.pending, (state) => {{
        state.status = 'loading';
      }})
      .addCase(fetch{controllerName}{endpointName}.fulfilled, (state, action) => {{
        state.status = 'succeeded';
        state.{controllerName.ToLower()}{endpointName}Data = action.payload;
      }})
      .addCase(fetch{controllerName}{endpointName}.rejected, (state, action) => {{
        state.status = 'failed';
        state.error = action.error.message;
      }});
  }},
}});

export default {controllerName.ToLower()}{endpointName}Slice.reducer;
";
    }

    static void GenerateModelFiles(string controllerName, string endpointName, MethodInfo method)
    {
        string responseModelFileName = $"Redux/controllers/{controllerName}/models/{controllerName}{endpointName}Response.ts";
        string paramsModelFileName = $"Redux/controllers/{controllerName}/models/{controllerName}{endpointName}Params.ts";

        // Generate the response model file content
        //string responseModelContent = GenerateModelContent(method.ReturnType, $"{controllerName}{endpointName}Response");

        // Generate the params model file content
        var parameters = method.GetParameters();
        if (parameters.Length > 0)
        {
            var paramType = parameters[0].ParameterType;
            string paramsModelContent = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramModelType = parameters[i];
                paramsModelContent = GenerateParamModelContent(paramModelType, $"{paramModelType.Name}Params");
            }



            // Write the params model content to the respective file
            File.WriteAllText(paramsModelFileName, paramsModelContent);
            Console.WriteLine($"Generated {paramsModelFileName}.");
        }

        // Write the response model content to the respective file
        //File.WriteAllText(responseModelFileName, responseModelContent);
        //Console.WriteLine($"Generated {responseModelFileName}.");
    }

    static string GenerateParamModelContent(ParameterInfo parameterModelType, string paramsModelFileName)
    {
        var modelContentBuilder = new StringBuilder();

        string propertyName = parameterModelType.Name;
        string propertyType = GetPropertyTypeForTs(parameterModelType.ParameterType);

        modelContentBuilder.AppendLine($"export interface {parameterModelType.Name} {{");

        modelContentBuilder.AppendLine($"  {parameterModelType.Name}: {propertyType};");

        modelContentBuilder.AppendLine("}");

        return modelContentBuilder.ToString();
    }

    static string GenerateModelContent(Type modelType, string modelName)
    {


        var properties = modelType.GetProperties();

        var modelContentBuilder = new StringBuilder();

        modelContentBuilder.AppendLine($"export interface {modelName} {{");

        foreach (var property in properties)
        {
            string propertyName = property.Name;
            string propertyType = GetPropertyTypeForTs(property.GetType());

            modelContentBuilder.AppendLine($"  {propertyName}: {propertyType};");
        }

        modelContentBuilder.AppendLine("}");

        return modelContentBuilder.ToString();
    }

    static string GetPropertyTypeForTs(Type propertyType)
    {
        if (propertyType == typeof(int) || propertyType == typeof(double) || propertyType == typeof(float) || propertyType == typeof(decimal))
        {
            return "number";
        }
        else if (propertyType == typeof(string) || propertyType == typeof(String))
        {
            return "string";
        }
        else if (propertyType == typeof(bool) || propertyType == typeof(Boolean))
        {
            return "boolean";
        }
        else if (propertyType.IsArray)
        {
            Type elementType = propertyType.GetElementType();
            return $"{GetPropertyTypeForTs(elementType)}[]";
        }
        else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
        {
            Type elementType = propertyType.GetGenericArguments()[0];
            return $"{GetPropertyTypeForTs(elementType)}[]";
        }
        else
        {
            // For complex types, use the type name as is
            return propertyType.Name;
        }
    }

    static void GenerateStoreFile(IEnumerable<Type> controllerTypes)
    {
        string storeFileName = "Redux/store.ts";
        var storeContentBuilder = new StringBuilder();

        storeContentBuilder.AppendLine("import { configureStore, combineReducers } from '@reduxjs/toolkit';");

        // Import the slices
        foreach (var controllerType in controllerTypes)
        {
            string controllerName = controllerType.Name.Replace("Controller", "");
            storeContentBuilder.AppendLine($"import {controllerName.ToLower()}Slices from './controllers/{controllerName}/{controllerName}Slices';");
        }

        // Combine the reducers
        storeContentBuilder.AppendLine("const rootReducer = combineReducers({");
        foreach (var controllerType in controllerTypes)
        {
            string controllerName = controllerType.Name.Replace("Controller", "");
            storeContentBuilder.AppendLine($"  {controllerName.ToLower()}: {controllerName.ToLower()}Slices,");
        }
        storeContentBuilder.AppendLine("});");

        // Configure the store
        storeContentBuilder.AppendLine("const store = configureStore({");
        storeContentBuilder.AppendLine("  reducer: rootReducer,");
        storeContentBuilder.AppendLine("});");

        storeContentBuilder.AppendLine("export default store;");

        // Write the store content to the store.ts file
        File.WriteAllText(storeFileName, storeContentBuilder.ToString());

        Console.WriteLine($"Generated {storeFileName}.");
    }
}