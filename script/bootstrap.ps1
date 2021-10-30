# Install dependencies that the application requires for building and running.
# Tested on Windows machine

echo "==> Installing required packages"
choco install visualstudio2019buildtools
choco install python3
choco install meson
choco install ninja

echo "==> Setting up meson environment"
meson setup builddir

echo "Bootstraping is done"
