// Splits large NHANES csv file into multiple small files.
// One file per subject id

#include <algorithm>  // std::find
#include <filesystem> // std::filesystem
#include <fstream>    // std::ifstream
#include <iostream>   // std::cout

namespace fs = std::filesystem;

// Command line arg parsing
// Based on https://stackoverflow.com/a/868894/706456
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

// Main
int main(int argc, char **argv) {

  // Arg validation
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

  std::string input_file_path(get_arg(argv, argv + argc, "-f"));
  std::string output_dir(get_arg(argv, argv + argc, "-o"));

  std::string line;
  std::ifstream in_stream;
  in_stream.open(input_file_path, std::ifstream::in);

  if (!in_stream.good()) {
    perror("Failed to open input file for reading");
    std::cout << "Current working directory: " << std::filesystem::current_path();
    exit(EXIT_FAILURE);
  }
  if (!fs::exists(output_dir)) {
    std::error_code ec;
    if (!fs::create_directories(output_dir, ec)) {
      std::cout << "Output dir: " << output_dir << " could not be created, error " << ec.value()
                << " (" << ec.message() + ").";
      exit(EXIT_FAILURE);
    }
  }

  std::string header_line;
  std::getline(in_stream, header_line);
  std::cout << "Header line:\n" << header_line << "\n";

  std::string cur_id;
  std::string new_id;

  std::ofstream *out_stream = nullptr;

  while (std::getline(in_stream, line)) {


    size_t first_comma = line.find_first_of(",");
    if (first_comma == std::string::npos)
      continue;
    first_comma++;
    size_t second_comma = line.find_first_of(",", first_comma);
    if (second_comma == std::string::npos)
      continue;

    new_id = line.substr(first_comma, second_comma - first_comma);

    if (cur_id != new_id) {
      if (out_stream != nullptr) {
        out_stream->close();
        delete out_stream;
        out_stream = nullptr;
      }

      cur_id = new_id;

      fs::path ouput_file(output_dir);
      ouput_file /= cur_id + ".csv";
      if (out_stream == nullptr) {
        out_stream = new std::ofstream(ouput_file.generic_string(), std::ofstream::out);
        if (!out_stream->good()) {
          perror("Failed to open ouput file for writing");
          std::cout << "Current working directory: " << std::filesystem::current_path() << ".\n"
                    << "File: " << ouput_file.generic_string();
          exit(EXIT_FAILURE);
        }
        *out_stream << header_line << "\n";
      }
    }
    *out_stream << line << "\n";
  }

  return 0;
}
