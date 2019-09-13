import React, { Component } from 'react';
import './ItemChurras.css';
import iconPeople from '../../assets/images/icons/icon_people.svg';
import iconMoney from '../../assets/images/icons/icon_money.svg';

class ItemChurras extends Component {
    render() {
        return (
            <div className="item-churras" onClick={this.props.onClick} >
                <h2>{this.props.dataChurras}</h2>
                <h3>{this.props.motivo}</h3>
                <div className="people">
                    <img src={iconPeople} />
                    <span>{this.props.quantidadePessoas}</span>
                </div>
                <div className="money">
                    <img src={iconMoney} />
                    <span>R$ {this.props.valorTotal}</span>
                </div>
            </div>
        );
    }
}

export default ItemChurras;