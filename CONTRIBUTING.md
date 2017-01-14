
Development
-----------

This has only been tested in _Visual Studio 2015 Community Edition_ and depends on _.NET 4.5.2_ - just open the solution and build, test or run.

Releasing
---------

This is rather manual and terrible:

1. Go to `MusicCollectionCompleter\Properties\AssemblyInfo.cs` and update `AssemblyVersion` and `AssemblyFileVersion` to a new version.
2. Clean and build the project with the `Release` profile.
3. Commit all changes, and tag the repo: `git tag -a v1.2 -m "Release 1.2"`
4. Push the tag: `git push --tags` and go to [the release page on GitHub](https://github.com/danielflower/MusicCollectionCompleter/releases) where the new tag should now be showing.
5. Edit the release, and upload a zip called `MusicCollectionCompleter-1.2.zip` of the release directory (all files can go in the root of the zip file) and publish the release.
6. Now go to the `gh-pages` branch and update the download link in `index.html`