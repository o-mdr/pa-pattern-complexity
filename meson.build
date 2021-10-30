project('pa-pattern-complexity', 'cpp', version: '0.0.1', license: 'Apache v2.0',
  default_options : [
    'cpp_std=c++2a',     # C++20
    'buildtype=release', # Release build
    'optimization=3',    # Optimize binary, choice is 0, g, 1, 2, 3, s
    'warning_level=3',   # Set warning level 0 (min) - 3 (max)
    'werror=true',       # Treat warning as errors
  ])

cxx = meson.get_compiler('cpp')
cxx_id = cxx.get_id()

compiler_gcc = 'gcc'
compiler_clang = 'clang'
compiler_msvc = 'msvc'

executable_version = meson.project_version()

message('Compiler id: @0@'.format(cxx_id))
message('Project version is now: @0@'.format(executable_version))

# =============================================================================
# Compiler flags
# =============================================================================

if [compiler_gcc, compiler_clang].contains(cxx_id)
  # https://caiorss.github.io/C-Cpp-Notes/compiler-flags-options.html#orgd8bc5f8
  common_warning_flags = [ '-Wpendantic', '-Wunused', '-Wconversion', '-Wundef',
    '-Wshadow', '-Wcast-align', '-Wcast-qual', '-Wold-style-cast',
    '-Wpointer-arith', '-Wmissing-prototypes',
    '-funroll-loops', '-fvectorize', '-fprofile-generate', '-fstack-protector-strong',
    '-fno-rtti', '-ffunction-sections', '-fdata-sections', '-Wl', '–gc-sections'
     ]
  if cxx_id == compiler_clang
    common_warning_flags += ['-Wno-sign-conversion', '-Wdocumentation']
  endif
  cxx_compile_flags = cxx.get_supported_arguments(common_warning_flags)
  message('Added gcc/clang options: @0@'.format(cxx_compile_flags))
  add_project_arguments(cxx_compile_flags, language : 'cpp')
elif cxx_id == compiler_msvc
  cxx_compile_flags = [ '/D_UNICODE', '/DUNICODE' ]
  message('Added msvc options: @0@'.format(cxx_compile_flags))
  add_project_arguments(cxx_compile_flags, language: 'cpp')
endif

exe = executable('nhanes-csv-split', 'src/nhanes-csv-split/main.cpp')
