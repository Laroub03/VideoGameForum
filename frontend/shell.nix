{pkgs ? import <nixpkgs> {}}:
with pkgs;
  mkShell {
    buildInputs = [
      bun
      nodePackages.npm
      nodePackages.sass
    ];
  }
