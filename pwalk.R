read.act = function(fname) {
  data = read.csv(fname, header = FALSE, col.names = c("X", "Y", "Z", "TIME"))
  data$TITLE = fname
  class(data) = c("activity", "data.frame")
  return(data)
}

plot.act = function(x, start = 0, end = max(x$TIME), sfrac = FALSE) {
  par(mfrow = c(3,1))
  x = x[which(x$TIME == min(x$TIME[x$TIME >= start])):which(x$TIME == min(x$TIME[x$TIME >= end])),]
  
  if(sfrac != FALSE) {
    plot(x$TIME, predict(loess(x$X ~ x$TIME, span = sfrac)), type = "l", col = "red", xlab = "Time", ylab = "X")
    plot(x$TIME, predict(loess(x$Y ~ x$TIME, span = sfrac)), type ="l", col = "green", xlab = "Time", ylab = "Y")
    plot(x$TIME, predict(loess(x$Z ~ x$TIME, span = sfrac)), type = "l", col = "blue", xlab = "Time", ylab = "Z")
  }
  else {
    plot(x$TIME, x$X, type = "l", col = "red", xlab = "Time", ylab = "X")
    plot(x$TIME, x$Y, type = "l", col = "green", xlab = "Time", ylab = "Y")
    plot(x$TIME, x$Z, type = "l", col = "blue", xlab = "Time", ylab = "Z")
  }
  return(x)
}

write.act = function(x, fname) {
  write.table(x, fname, sep = ",", row.names = FALSE, col.names = FALSE)
}

plot.act.single = function(x, color) {
  plot(x$TIME, x$Y, col = color, ylab = "Y", xlab = "Time (msec)", main = x$TITLE[1], type = "l", lwd = 3)
}