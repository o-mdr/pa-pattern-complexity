#!/usr/bin/Rscript
# This script converts XPT file into CSV file using R

library('foreign')

args <- commandArgs(trailingOnly = TRUE)

xpt_to_csv <- function(src_path, dst_path) {
  print("r: WARNING this code isn't memory efficient, it stores XPS file in memory twice")
  print(paste("r: reading full XPS file into memory from:", src_path, sep = " "))
  data = read.xport(src_path)

  print(paste("r: writing data to csv file:", dst_path, sep = " "))
  write.csv(data, file = dst_path)
}

xpt_to_csv(src_path = args[1], dst_path = args[2])
