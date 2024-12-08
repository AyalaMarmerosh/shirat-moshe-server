{ pkgs }:
pkgs.mkShell {
  buildInputs = [
    pkgs.dotnet-sdk_7
  ];
}
