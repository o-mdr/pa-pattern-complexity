#pragma once

#include <algorithm> // std::find
#include <iostream>  // std::cout

// Command line arg parsing
// Based on https://stackoverflow.com/a/868894/706456
class arg_parser {

public:
  char *get_arg(char **begin, char **end, const std::string &option) {
    auto it = std::find(begin, end, option);
    if (it != end && ++it != end)
      return *it;
    return nullptr;
  }

  bool arg_exists(char **begin, char **end, const std::string &option) {
    return std::find(begin, end, option) != end;
  }

  void print_help() {
    std::cout
        << "This program splits large NHANES csv file into multiple small files.\n"
        << "Arguments: \n"
        << "  -f        path to the single large csv file\n"
        << "  -o        path to the destination directory where smaller files will be written\n"
        << "Example usage: \n"
        << "  Linux:    ./nhanes-csv-split   -f \"data/paxraw_c.xpt.csv\" -o \"data/study-c\"\n"
        << "  Windows:  nhanes-csv-split.exe -f \"data\\paxraw_c.xpt.csv\" -o \"data\\study-c\"\n";
  }

  void validate(int argc, char **argv) {
    if (argc == 1 || arg_exists(argv, argv + argc, "-h")) {
      print_help();
      exit(EXIT_SUCCESS);
    }
    if (!arg_exists(argv, argv + argc, "-f")) {
      std::cout << "-f argument is missing. See usage with -h argument.";
      exit(EXIT_SUCCESS);
    }
    if (!arg_exists(argv, argv + argc, "-o")) {
      std::cout << "-o argument is missing. See usage with -h argument.";
      exit(EXIT_SUCCESS);
    }
  }
};
