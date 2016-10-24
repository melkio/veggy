properties {
	$current_directory = Resolve-Path .
	$solution_directory = Join-Path $current_directory -ChildPath "src"
	$packages_folder = Join-Path $solution_directory -ChildPath "packages"

	# tools
	$nuget = Join-Path $current_directory -ChildPath "tools\nuget\nuget.exe"
	$nunit = Join-Path $current_directory -ChildPath "tools\nunit\NUnit.ConsoleRunner\tools\nunit3-console.exe"
	
	#config
	$test_project_filter = "*Test" 
	$config = "Release"
	$build_number = "0"

	#options
	$clean_packages = "false"
}

Framework "4.5.2"

task clean -depends clean_packages, clean_solutions 
task build -depends clean, build_solutions

task dev -depends run_tests

task default -depends dev

task clean_packages -precondition { return $clean_packages -eq "true" } {
	$exists = Test-Path $packages_folder
	If ($exists -eq $True) {
		Remove-Item $packages_folder -Force -Recurse 
	}
}

task clean_solutions {
	Get-ChildItem $solution_directory -Filter "*.sln" |
		ForEach-Object {
			Exec { msbuild $_.fullname /t:Clean /p:Configuration=$config /v:quiet /ds }
        }
}

task restore_nuget_packages {
	Get-ChildItem -Path $solution_directory -Filter "packages.config" -Recurse |
		ForEach-Object {
			Exec { . $nuget install $_.fullname -OutputDirectory $packages_folder -NonInteractive }
		}
}

task build_solutions -depends restore_nuget_packages {
	Get-ChildItem $solution_directory -Filter "*.sln" |
		ForEach-Object {
			Exec { msbuild $_.fullname /t:Build /p:Configuration=$config /v:quiet /ds }
        }	
}

task run_tests -depends build {
	Get-ChildItem $solution_directory -Filter $test_project_filter -Recurse |
		ForEach-Object {
			$fullname = $_.fullname
			$name = $_.name
			Exec { . $nunit "$fullname\bin\$config\$name.dll" }
        }	
}