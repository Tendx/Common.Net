param([string]$Project)

# funtions
function Quit {
    param (
        [string]$msg
    )
    if ($msg -ne "" | Out-Null) {
        Write-Host $msg
    }
    Write-Host "<EXIT"
    Exit
}

function CreateDir {
    param (
        [string]$dir
    )
    Write-Host " create directory: " $dir
    if (Test-Path $dir) {
        Quit(" directory has existed")
    }
    New-Item -ItemType Directory -Path $dir | Out-Null
}

function CreateFile {
    param (
        [string]$path,
        [string]$content
    )
    Write-Host " create and write file: " $path
    Set-Content -Path $path -Value $content -Encoding UTF8
}

# Start
Write-Host ">START"
if ($Project -eq "") {
    Quit
}
Write-Host " project: " $Project
$ProjectDir = "..\" + $Project
$ProjectGuid = [guid]::NewGuid().Guid

# Create Dirs
Write-Host "#create directories"
CreateDir "$ProjectDir"
CreateDir "$ProjectDir\MVVM"
CreateDir "$ProjectDir\Properties"

# Create and Write Files
Write-Host "#create and write files"
## Level 1
CreateFile "$ProjectDir\App.config" @"
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
</configuration>
"@

CreateFile "$ProjectDir\App.xaml" @"
<Application x:Class="$Project.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="ViewBase.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
"@

CreateFile "$ProjectDir\App.xaml.cs" @"
using System;
using System.Windows;

namespace $Project
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Action<string> _log;
        public static void Log(string text, bool newline = true) => _log(text + (newline ? Environment.NewLine : ""));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var vmApp = new MVVM.ViewModelApp();
            _log += text => vmApp.Model.Text += text;
            vmApp.Render();
        }
    }
}
"@

CreateFile "$ProjectDir\$Project.csproj" @"
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="`$(MSBuildExtensionsPath)\`$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('`$(MSBuildExtensionsPath)\`$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '`$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '`$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{$ProjectGuid}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <RootNamespace>$Project</RootNamespace>
    <AssemblyName>$Project</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <ProjectTypeGuids>{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
    <WarningLevel>4</WarningLevel>
    <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition=" '`$(Configuration)|`$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '`$(Configuration)|`$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Xml" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Xaml">
      <RequiredTargetFramework>4.0</RequiredTargetFramework>
    </Reference>
    <Reference Include="WindowsBase" />
    <Reference Include="PresentationCore" />
    <Reference Include="PresentationFramework" />
  </ItemGroup>
  <ItemGroup>
    <ApplicationDefinition Include="App.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </ApplicationDefinition>
    <Compile Include="App.xaml.cs">
      <DependentUpon>App.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="MVVM\ViewModelApp.cs" />
    <Compile Include="MVVM\ViewApp.xaml.cs">
      <DependentUpon>ViewApp.xaml</DependentUpon>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <Compile Include="MVVM\ModelText.cs" />
    <Compile Include="Properties\AssemblyInfo.cs">
      <SubType>Code</SubType>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <None Include="App.config" />
  </ItemGroup>
  <ItemGroup>
    <Page Include="MVVM\ViewApp.xaml">
      <SubType>Designer</SubType>
      <Generator>MSBuild:Compile</Generator>
    </Page>
  </ItemGroup>
  <Import Project="..\Zen.Wpf\Zen.Wpf.projitems" Label="Shared" />
  <Import Project="`$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
"@

## Level 2
CreateFile "$ProjectDir\Properties\AssemblyInfo.cs" @"
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("$Project")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("$Project")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

//若要开始生成可本地化的应用程序，请设置
//.csproj 文件中的 <UICulture>CultureYouAreCodingWith</UICulture>
//例如，如果您在源文件中使用的是美国英语，
//使用的是美国英语，请将 <UICulture> 设置为 en-US。  然后取消
//对以下 NeutralResourceLanguage 特性的注释。  更新
//以下行中的“en-US”以匹配项目文件中的 UICulture 设置。

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //主题特定资源词典所处位置
                                     //(未在页面中找到资源时使用，
                                     //或应用程序资源字典中找到时使用)
    ResourceDictionaryLocation.SourceAssembly //常规资源词典所处位置
                                              //(未在页面中找到资源时使用，
                                              //、应用程序或任何主题专用资源字典中找到时使用)
)]


// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
"@

CreateFile "$ProjectDir\MVVM\ModelText.cs" @"
using System;
using System.Xml.Serialization;

namespace $Project.MVVM
{
    [Serializable]
    public class ModelText : Zen.ModelBase
    {
        private string _text;

        [XmlAttribute]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
"@

CreateFile "$ProjectDir\MVVM\ViewApp.xaml" @"
<Window x:Class="$Project.MVVM.ViewApp"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:z="clr-namespace:Zen"
        mc:Ignorable="d"
        Title="🗔" Height="400" Width="800" Style="{StaticResource ViewBaseStyle}">
    <Grid>
        <ListBox Style="{StaticResource ControlPanel}">
            <Button Content="✓" z:Route.Path="Click->Accept"/>
            <Button Content="✗" z:Route.Path="Click->Leave"/>
        </ListBox>
        <TextBox IsReadOnly="True" TextWrapping="Wrap" VerticalScrollBarVisibility="Auto" Text="{Binding Model.Text}"
                 z:Route.Path="TextChanged->ScrollToEnd" z:Route.Redirect="{Binding RelativeSource={RelativeSource Self}}"/>
    </Grid>
</Window>
"@

CreateFile "$ProjectDir\MVVM\ViewApp.xaml.cs" @"
using System.Windows;

namespace $Project.MVVM
{
    /// <summary>
    /// ViewApp.xaml 的交互逻辑
    /// </summary>
    public partial class ViewApp : Window
    {
        public ViewApp()
        {
            InitializeComponent();
        }
    }
}
"@

CreateFile "$ProjectDir\MVVM\ViewModelApp.cs" @"
using System;

namespace $Project.MVVM
{
    public class ViewModelApp : Zen.ViewModelBase<ModelText>
    {
        public ViewModelApp(ModelText model = null) : base(model)
        {
        }

        public void Accept(object s, object e)
        {
            App.Log("Accept");
        }
    }
}
"@

# End
Quit