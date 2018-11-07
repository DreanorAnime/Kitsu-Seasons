git submodule update --recursive --remote

nuget restore src\submodules\Kitsu\Kitsu.sln
nuget restore src\submodules\Library\Library.sln
nuget restore src\kitsuseasons.sln