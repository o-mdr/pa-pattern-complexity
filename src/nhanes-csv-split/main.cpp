// Splits large NHANES csv file into multiple small files.
// One file per subject id

#include <filesystem> // std::filesystem
#include <fstream>    // std::ifstream
#include <iomanip>    // setprecision
#include <iostream>   // std::cout
#include <sys/stat.h> // stat

#include "arg_parser.h"

namespace fs = std::filesystem;

long file_size(const std::string &filename) {
  struct stat st;
  int rc = stat(filename.c_str(), &st);
  return rc == 0 ? st.st_size : -1;
}

#define B_TO_KB(x) (static_cast<size_t>(x) >> 10)
#define B_TO_MB(x) (static_cast<size_t>(x) >> 20)
#define B_TO_GB(x) (static_cast<size_t>(x) >> 30)

int main(int argc, char **argv) {
  arg_parser args;
  args.validate(argc, argv);

  std::string input_file_path(args.get_arg(argv, argv + argc, "-f"));
  std::string output_dir(args.get_arg(argv, argv + argc, "-o"));

  std::string line;
  std::ifstream in_stream;
  in_stream.open(input_file_path, std::ifstream::in);

  if (!in_stream.good()) {
    perror("Failed to open input file for reading");
    std::cout << "Current working directory: " << std::filesystem::current_path() << "\n"
    << "Input file: " << input_file_path << "\n";
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

  std::cout << "Begin splitting: " << input_file_path << "\n";
  std::string header_line;
  std::getline(in_stream, header_line);
  std::cout << "Header line: " << header_line << "\n";

  std::string cur_id;
  std::string new_id;

  long number_of_out_files = 0;
  long processed_size = 0;
  long input_size = file_size(input_file_path);
  if (input_size < 0) {
    std::cout << "Failed to get input file size";
    exit(EXIT_FAILURE);
  }

  processed_size += header_line.length() * sizeof(char);

  std::ofstream *out_stream = nullptr;

  while (std::getline(in_stream, line)) {
    processed_size += line.length() * sizeof(char);
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
        number_of_out_files++;

        if (number_of_out_files % 50 == 0) {
          std::cout << "Created " << number_of_out_files << " files, "
                    << "processed " << B_TO_MB(processed_size) << " / " << B_TO_MB(input_size)
                    << " MB (" << std::setprecision(3)
                    << (processed_size * 100 / input_size) << "%)" << std::endl;
        }
      }
    }
    *out_stream << line << "\n";
  }

  std::cout << "Generated " << number_of_out_files << " files.\n";
  std::cout << "Convertsion is finised, see your data in: " << output_dir << ".\n";
  return 0;
}
