FROM mcr.microsoft.com/devcontainers/dotnet:latest@sha256:d99e4e4a3de2bf9249b985e7cb05024d9227361bd346611c0b7f352360c4dc3b

# Suppress an apt-key warning about standard out not being a terminal. Use in this script is safe.
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=DontWarn

# Install Node.js
RUN su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install lts/* 2>&1"

# Install Google Chrome
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | sudo apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' | sudo tee /etc/apt/sources.list.d/google-chrome.list
RUN sudo apt-get update
RUN sudo apt-get --yes install google-chrome-stable 2>&1

# Install Firefox
RUN sudo apt-get --yes install firefox 2>&1
