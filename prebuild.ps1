git submodule update --init --recursive --remote #--init is a workaround for appveyor

nuget restore src\submodules\Kitsu\src\Kitsu.sln
nuget restore src\submodules\Library\src\Library.sln
nuget restore src\kitsuseasons.sln