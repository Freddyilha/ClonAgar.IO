const express = require('express')
const path = require('path')
const http = require('http')
const socketIO = require('socket.io')

const app = express()
const server = http.createServer(app)
const io = socketIO(server)

const port = 7777


io.on('connection', (socket) => {
    console.log('Someone at' + socket.id + ' connected')

    socket.emit('successfull')

    socket.on('updatePosition', (obj) => {
        const data = JSON.stringify(obj)
        console.log('recebi' + data)
        console.log('valeu pela posição')

        io.emit('MoveEnemies', data)
    })


    socket.on('disconnect', function () {
        console.log('desconectado')
    })
})

server.listen(port, () => {
    console.log('Server is up on port ' + port)
})