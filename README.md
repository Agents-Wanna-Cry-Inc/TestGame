# Game

![Screenshot](/screenshot.png)

The central repository for the next groundbreaking game of 2020. With accompanying [trello board](https://trello.com/b/YAWo8IqC/game) of course.

### Technical info

* Uses Unity 2019.3.4f1

### Issues

* ~~[`qoc7yp`] Player is too slippery causing it to slide down on a sloped surface~~
* ~~[`o1bzcz`] Player may unexpectedly slide of platforms with rounded corners due too slippery surface~~
* ~~[`b41yhi`] Background may not fill the entire view (Unity issue?)~~
* ~~[`jf8b3y`] Tilemap collider not generated correctly for slightly irregular object, causing objects to float~~
* ~~[`s9f2fv`] Player is not grounded when standing on interactable objects~~
* ~~[`9v8y3g`] Jump animation not lined up with the actual jump~~
* ~~[`f84j3x`] Player idle animation is too fast, even though the framerate is very low~~

### Known limitations

* When using the `PlayerController` script or using the `AutoZoom` script certain properties such as `Gravity Scale` and `Orthographic Size` cannot be edited while running the game.
* Block objects may clip throught the floor due to their `discrete` rigidbody; using `continious` would fix this but the performance penalty is not worth it.
