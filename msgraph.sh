docker run -it --rm -v ${PWD}:/local diegomvh/odataapigen \
  Name=MsGraph \
  Metadata=https://graph.microsoft.com/v1.0/\$metadata \
  Output=/local