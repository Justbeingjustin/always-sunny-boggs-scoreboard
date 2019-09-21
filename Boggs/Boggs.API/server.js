var promise = require('bluebird');
var options = {
  // Initialization Options
  promiseLib: promise
};
var cors = require('cors')

var pgp = require('pg-promise')(options);
var connectionString = 'CONNECTION_STRING';
var db = pgp(connectionString);


var express = require("express");
var bodyParser = require('body-parser');
var app = express();
app.use(bodyParser.json());
app.use(cors());

// sets port 8080 to default or unless otherwise specified in the environment
app.set('port', process.env.PORT || 8080);

app.get('/api/jobs', function(req, res,next){
    console.log('made it this far!')
    db.any('select * from JOBS where status=0')
      .then(function (data) {
        res.status(200)
          .json({
            status: 'success',
            data: data,
            message: 'Retrieved ALL Jobs'
          });
      })
      .catch(function (err) {
        return next(err);
      });
});

app.get('/api/job/:jobId', function(req, res,next){

  db.any('select * from JOBS where jobId=' + req.params.jobId)
    .then(function (data) {
      res.status(200)
        .json({
          status: 'success',
          data: data,
          message: 'Retrieved ALL Jobs'
        });
    })
    .catch(function (err) {
      return next(err);
    });
});


app.put('/api/job', function (req, res,next) {

    var jobId=req.body.jobId;
    var status = req.body.status;
    var statusMessage = req.body.statusMessage;

var updateStatement ='UPDATE JOBS SET status='+status+', statusMessage=\'' + statusMessage + '\' where jobid=' + jobId
db.any(updateStatement,
    req.body)
    .then(function (data) {
      res.status(200)
        .json({
          status: 'success',
          message: 'Updated one job'
        });
    })
    .catch(function (err) {
      return next(err);
    });
});



app.post('/api/job', function (req, res,next) {
   
  
    var paramaters = JSON.stringify(req.body.data);
    var status = req.body.status;
    var statusMessage = req.body.statusMessage;
    var createdDate = req.body.createdDate;

var insertStatement = 'INSERT INTO JOBS (PARAMATERS,STATUS,STATUSMESSAGE,CREATEDDATE)' +
' VALUES (\''+paramaters+'\', \''+status+'\', \''+statusMessage+'\', \''+createdDate+'\') RETURNING JOBID;;';
console.log(insertStatement);
    db.any(insertStatement,
      req.body)
      .then(function (data) {
        res.status(200)
          .json({
            status: 'success',
            jobId: data[0].jobid,
            message: 'Inserted one job'
          });
      })
      .catch(function (err) {
        return next(err);
      });

      
  })

app.listen(app.get('port'));