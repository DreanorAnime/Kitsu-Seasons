git submodule update --init --recursive --remote #--init is a workaround for appveyor

nuget restore src\submodules\Kitsu\Kitsu.sln
nuget restore src\submodules\Library\Library.sln
nuget restore src\kitsuseasons.sln