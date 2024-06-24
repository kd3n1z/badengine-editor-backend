namespace badengine_editor_backend;

public static class ProjectHelper {
    private const string ProjectTemplate = @"<Project Sdk=""Microsoft.NET.Sdk"">
    <ItemGroup>
        <Compile Include=""assets/**/*.cs"" />
    </ItemGroup>

    <ItemGroup>
        <ProjectReference Include=""{0}"">
            <Private>True</Private>
        </ProjectReference>
    </ItemGroup>

    <PropertyGroup>
        <OutputType>library</OutputType>
        <TargetFramework>net6.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    </PropertyGroup>
</Project>";
    
    public static string GenerateCsProj(string libraryPath) {
        return string.Format(ProjectTemplate, Path.Combine(libraryPath, "badengine.csproj"));
    }
}